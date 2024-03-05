using artshare_server.Core.Interfaces;
using AutoMapper;

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
        private readonly IOrderDetailsRepository _orderDetailsRepo;
        private readonly IReportRepository _reportRepo;
        private readonly IWatermarkRepository _watermarkRepo;
        private readonly IMapper _mapper;
        public UnitOfWork(IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = new AppDbContext();
            _accountRepo = new AccountRepository(_dbContext);
            _artworkRepo = new ArtworkRepository(_dbContext);
            _commentRepo = new CommentRepository(_dbContext);
            _genreRepo = new GenreRepository(_dbContext);
            _likeRepo = new LikeRepository(_dbContext);
            _orderRepo = new OrderRepository(_dbContext,_mapper);
            _orderDetailsRepo = new OrderDetailsRepository(_dbContext,_mapper);
            _reportRepo = new ReportRepository(_dbContext);
            _watermarkRepo = new WatermarkRepository(_dbContext);
        }

        public IAccountRepository AccountRepo => _accountRepo;
        public IArtworkRepository ArtworkRepo => _artworkRepo;
        public ICommentRepository CommentRepo => _commentRepo;
        public IGenreRepository GenreRepo => _genreRepo;
        public ILikeRepository LikeRepo => _likeRepo;
        public IOrderRepository OrderRepo => _orderRepo;
        public IOrderDetailsRepository OrderDetailsRepo => _orderDetailsRepo;
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