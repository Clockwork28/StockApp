﻿using StockApp.DTOs.Comment;
using StockApp.Models;

namespace StockApp.Mappers
{
    public static class CommentMappers
    {
        public static CommentDTO ToCommentDTO(this Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                CreatedBy = comment.AppUser.UserName,
                StockId = comment.StockId
            };
        }

        public static Comment ToCommentFromRequest(this CreateCommentRequest request, Guid stockId)
        {
            return new Comment
            {
                Title = request.Title,
                Content = request.Content,
                StockId = stockId,
                
            };
        }
    }
}
