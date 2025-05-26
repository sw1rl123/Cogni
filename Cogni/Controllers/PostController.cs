﻿using Cogni.Abstractions.Services;
using Cogni.Authentication.Abstractions;
using Cogni.Contracts.Requests;
using Cogni.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ITokenService _tokenService;

        public PostController(IPostService postService, ITokenService tokenService)
        {
            _postService = postService;
            _tokenService = tokenService;
        }
        /// <summary>
        /// Создание поста на стене пользователя
        /// </summary>
        /// <remarks>Нужно в теле сообщения отправить файл/файлы (Key = Picture, multipart/form-data). Поддерживаются форматы JPEG/JPG, PNG, GIF, BMP, TIFF, SVG, WebP, HEIF/HEIC, ICO, RAW.</remarks>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult<PostResponse>> CreatePost([FromForm] PostRequest post)
        {
            string token = Request.Headers["Authorization"];
            token = token.Replace("Bearer ", string.Empty);
            int id = _tokenService.GetTokenPayload(token).UserId;



            var p = await _postService.CreatePost(post, id);

            var list = p.PostImages.ToList();
            List<string> urls = new List<string>();
            foreach (var image in list)
            {
                urls.Add(image.ImageUrl);
            }
            return Ok(new PostResponse(p.Id, p.PostBody,p.CreatedAt, p.IdUser, urls));

        }
        /// <summary>
        /// Удаление поста со стены
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeletePost(int id)
        {
            await _postService.DeletePost(id);
            return Ok();
        }
        /// <summary>
        /// Получение всех постов пользователя
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult<List<PostResponse>>> GetAllUserPost(int id)
        {
            var posts = await _postService.GetAllUserPosts(id);
            var list = new List<PostResponse>();
            foreach (var post in posts)
            {
                var urls = post.PostImages.Select(u => u.ImageUrl).ToList();
                list.Add(new PostResponse(post.Id, post.PostBody, post.CreatedAt, post.IdUser, urls));
            }
            return Ok(list);
        }

        
    } 
}
