using System.ComponentModel.Design;
using SocialNetwork.Core;
using SocialNetwork.Core.Entities;
using SocialNetwork.Core.Interfaces;

namespace SocialNetwork.Infrastructure;
public class CommentService : ICommentService
{
    private readonly IRepository<Post> postRepository;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<Comment> commentRepository;

    public CommentService(
        IRepository<Post> postRepository,
        IRepository<User> userRepository,
        IRepository<Comment> commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public OperationResult<Comment> Create(Comment comment)
    {
        var post = this.postRepository.GetById(comment.PostId);
        if (post is null)
        {
            return new OperationResult<Comment>(new Error
            {
                Message = $"No se encontró un post con id {comment.PostId} para agregar el comentario",
                Code = ErrorCode.BadRequest
            });
        }
        var user = this.userRepository.GetById(comment.UserId);
        if (user is null)
        {
            return new OperationResult<Comment>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = $"No se encontró un usuario con id {comment.UserId} para agregar el comentario"
            });
        }

        if (string.IsNullOrEmpty(comment.Content))
        {
            return new OperationResult<Comment>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = "El comentario debe tener contenido."
            });
        }

        this.commentRepository.Add(comment);
        return new OperationResult<Comment>(comment);
    }

    public OperationResult<Comment> GetById(int id, int postId)
    {
        var post = this.postRepository.GetById(postId);
        if (post is null)
        {
            return new OperationResult<Comment>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = $"No se encontró un post con id {postId}"
            });
        }

        var comment = this.commentRepository.GetById(id);
        if (comment is null)
        {
            return new OperationResult<Comment>(new Error
            {
                Code = ErrorCode.BadRequest,
                Message = $"No se encontró un comentario con el id {postId}"
            });
        }

        return comment;
    }
}

