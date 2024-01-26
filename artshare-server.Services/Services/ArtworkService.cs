using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.Interfaces;

namespace artshare_server.Services.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArtworkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Artwork>> GetAllArtworksAsync()
        {
            var artworkList = await _unitOfWork.ArtworkRepo.GetAllAsync();
            return artworkList;
        }

        public async Task<Artwork> GetArtworkByIdAsync(int artworkId)
        {
            if (artworkId > 0)
            {
                var artwork = await _unitOfWork.ArtworkRepo.GetByIdAsync(artworkId);
                if (artwork != null)
                {
                    return artwork;
                }
            }
            return null;
        }

        public async Task<bool> CreateArtworkAsync(Artwork artwork)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateArtworkAsync(Artwork artwork)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteArtworkAsync(int artworkId)
        {
            throw new NotImplementedException();
        }
    }
}