using artshare_server.Core.Interfaces;

namespace artshare_server.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IAccountRepository _accountRepo;
        private readonly IArtworkRepository _artworkRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IGenreRepository _genreRepo;
        private readonly ILikeRepository _likeRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IReportRepository _reportRepo;
        private readonly IWatermarkRepository _watermarkRepo;

        public UnitOfWork(AppDbContext dbContext, IAccountRepository accountRepo, IArtworkRepository artworkRepo, ICommentRepository commentRepo,
            IGenreRepository genreRepo, ILikeRepository likeRepo, IOrderRepository orderRepo, IReportRepository reportRepo, IWatermarkRepository watermarkRepo)
        {
            _dbContext = dbContext;
            _accountRepo = accountRepo;
            _artworkRepo = artworkRepo;
            _commentRepo = commentRepo;
            _genreRepo = genreRepo;
            _likeRepo = likeRepo;
            _orderRepo = orderRepo;
            _reportRepo = reportRepo;
            _watermarkRepo = watermarkRepo;
        }

        public IAccountRepository AccountRepo => _accountRepo;
        public IArtworkRepository ArtworkRepo => _artworkRepo;
        public ICommentRepository CommentRepo => _commentRepo;
        public IGenreRepository GenreRepo => _genreRepo;
        public ILikeRepository LikeRepo => _likeRepo;
        public IOrderRepository OrderRepo => _orderRepo;
        public IReportRepository ReportRepo => _reportRepo;
        public IWatermarkRepository WatermarkRepo => _watermarkRepo;

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}