﻿using Microsoft.AspNetCore.Mvc;
using Cogni.Contracts;
using Cogni.Services;
using Cogni.Contracts.Responses;
using Cogni.Database.Entities;
using Cogni.Abstractions.Services;
using Cogni.Contracts.Requests;
using Cogni.Authentication.Abstractions;
using Cogni.Models;
using System.Web.Http;

namespace Cogni.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService, ITokenService tokenService, ILogger<ArticleController> logger)
        {
            _logger = logger;
            _articleService = articleService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Получение всех заголовков статей
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<List<ArticleResponse>>> GetArticles()
        {
            var articles = await _articleService.GetAllAsync();

            return Ok(articles);
        }

        /// <summary>
        /// Получение всех данных статьи
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<ArticleResponse>> GetArticleById(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            await _articleService.IncrementArticleReadsAsync(id);

            article = await _articleService.GetArticleByIdAsync(id);

            var urls = article.ArticleImages?.Select(i => i.ImageUrl).ToList() ?? new List<string>();
            var articleResponse = new ArticleResponse(article.Id, article.ArticleName, article.ArticleBody, urls, article.IdUser, article.Annotation, article.Created, article.ReadsNumber, article.ArticlePreview);

            return Ok(articleResponse);

        }

        /// <summary>
        /// Создание статьи
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Authorize]
        public async Task<ActionResult<ArticleResponse>> CreateArticle([FromForm] CreateArticleRequest request)
        {
            if (string.IsNullOrEmpty(request.ArticleName) || string.IsNullOrEmpty(request.ArticleBody) || string.IsNullOrEmpty(request.Annotation))
            {
                return BadRequest("Название, текст или аннотация статьи не могут быть пустыми");
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            int userId = _tokenService.GetTokenPayload(token).UserId;


            try
            {
                var createdArticle = await _articleService.CreateArticleAsync(request, userId);

                var imageUrls = createdArticle.ArticleImages.Select(i => i.ImageUrl).ToList();
                var articleResponse = new ArticleResponse(createdArticle.Id, createdArticle.ArticleName, createdArticle.ArticleBody, imageUrls, createdArticle.IdUser, createdArticle.Annotation, createdArticle.Created, createdArticle.ReadsNumber, createdArticle.ArticlePreview);

                return CreatedAtAction(nameof(GetArticleById), new { id = createdArticle.Id }, articleResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SixLabors.ImageSharp.UnknownImageFormatException ex)
            {
                return BadRequest("Неподдерживаемый формат изображения");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка при создании статьи");
            }
        }


        /// <summary>
        /// Изменение статьи
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPut]
        [Authorize]
        public async Task<ActionResult<ArticleResponse>> UpdateArticle([FromForm] ArticleUpdateRequest request)
        {
            if (request.IdArticle <= 0)
            {
                return BadRequest("Некорректный ID статьи");
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            int userId = _tokenService.GetTokenPayload(token).UserId;

            try
            {
                var updatedArticle = await _articleService.UpdateArticleAsync(request.IdArticle, request, userId);

                var imageUrls = updatedArticle.ArticleImages.Select(i => i.ImageUrl).ToList();
                var articleResponse = new ArticleResponse(updatedArticle.Id, updatedArticle.ArticleName, updatedArticle.ArticleBody, imageUrls, updatedArticle.IdUser, updatedArticle.Annotation, updatedArticle.Created, updatedArticle.ReadsNumber, updatedArticle.ArticlePreview);

                return Ok(articleResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SixLabors.ImageSharp.UnknownImageFormatException ex)
            {
                return BadRequest("Неподдерживаемый формат изображения");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Произошла ошибка при изменении статьи");
            }
        }

        /// <summary>
        /// Превью статьи
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<ArticlePreviewResponse>> GetArticlesPreviews()
        {
            var preview = await _articleService.GetArticlesPreviewesAsync();
            return Ok(preview);
        }

        /// <summary>
        /// Удаление статьи
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteArticle(int id)
        {
            await _articleService.DeleteArticleAsync(id);
            return NoContent();
        }
    }
}