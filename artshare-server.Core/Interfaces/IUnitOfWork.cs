namespace artshare_server.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepo { get; }
        IArtworkRepository ArtworkRepo { get; }
        ICommentRepository CommentRepo { get; }
        IGenreRepository GenreRepo { get; }
        ILikeRepository LikeRepo { get; }
        IOrderRepository OrderRepo { get; }
        IOrderDetailsRepository OrderDetailsRepo { get; }
        IReportRepository ReportRepo { get; }
        IWatermarkRepository WatermarkRepo { get; }

        Task<int> SaveAsync();
    }
}