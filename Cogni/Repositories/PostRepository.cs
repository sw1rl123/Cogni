﻿using Cogni.Abstractions.Repositories;
using Cogni.Database.Context;
using Cogni.Models;
using Microsoft.EntityFrameworkCore;
using Cogni.Database.Entities;

namespace Cogni.Database.Repositories;
public class PostRepository : IPostRepository
{
    private readonly CogniDbContext _context;
    public PostRepository(CogniDbContext context)
    {
        _context = context;
    }
    public async Task<Post> CreatePost(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post;
    }

    public async Task DeletePost(int id)
    {
        Post post = await _context.Posts.FindAsync(id);
        _context.Remove(post);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Post>> GetAllUserPosts(int id)
    {
        var posts = await _context.Posts
            .Include(a => a.PostImages)
            .Where(u => u.IdUser == id)
            .ToListAsync();
        return posts;
    }

    public async Task<Post> UpdatePost(Post post)//TODO
    {
        Post up = new Post { Id = post.Id , IdUser = post.IdUser, PostBody = post.PostBody, PostImages = post.PostImages};
        await _context.SaveChangesAsync();
        return up;

    }
}

