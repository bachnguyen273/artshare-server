using artshare_server.ApiModels.DTOs;
using artshare_server.Core.Interfaces;
using artshare_server.Core.Models;
using artshare_server.Services.FilterModels;
using artshare_server.Services.FilterModels.Helpers;
using artshare_server.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace artshare_server.Services.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageService _azureBlobStorageService;

        public ArtworkService(IUnitOfWork unitOfWork, IMapper mapper, IAzureBlobStorageService azureBlobStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _azureBlobStorageService = azureBlobStorageService;
        }

        //public async Task<IEnumerable<Artwork>> GetAllArtworksAsync()
        //{
        //    var artworkList = await _unitOfWork.ArtworkRepo.GetAllAsync();
        //    return artworkList;
        //}

        public async Task<GetArtworkDTO?> GetArtworkByIdAsync(int artworkId)
        {
            if (artworkId > 0)
            {
                var artwork = await _unitOfWork.ArtworkRepo.GetByIdAsync(artworkId);
                return _mapper.Map<GetArtworkDTO>(artwork);
            }
            return null;
        }

        public async Task<bool> UpdateArtworkAsync(Artwork artwork)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteArtworkAsync(int artworkId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateArtworkAsync(CreateArtworkDTO createArtworkDTO)
        {
            Artwork artwork = _mapper.Map<Artwork>(createArtworkDTO);
            artwork.CommentCount = 0;
            artwork.LikeCount = 0;
            artwork.DislikeCount = 0;
            artwork.CreatedDate = DateTime.Now;
            artwork.OriginalArtUrl = createArtworkDTO.OriginalArtUrl;
            await _unitOfWork.ArtworkRepo.AddAsync(artwork);
            var result = await _unitOfWork.SaveAsync() > 0;
            return result;
        }

        public async Task<PagedResult<GetArtworkDTO>> GetAllArtworksAsync<T>(ArtworkFilters filters)
        {
            // Apply filtering
            var items = await _unitOfWork.ArtworkRepo.GetArtworks();
            IQueryable<GetArtworkDTO> filteredItemsQuery = items.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Title))
                filteredItemsQuery = filteredItemsQuery.Where(item => item.Title.Contains(filters.Title, StringComparison.OrdinalIgnoreCase));
            if (filters.CreatorId != null)
                filteredItemsQuery = filteredItemsQuery.Where(item => item.CreatorId == filters.CreatorId);
            if (filters.ArtworkId != null)
                filteredItemsQuery = filteredItemsQuery.Where(item => item.ArtworkId == filters.ArtworkId);

            // Apply sorting
            if (!string.IsNullOrEmpty(filters.SortBy))
            {
                switch (filters.SortBy)
                {
                    case "price":
                        filteredItemsQuery = filters.SortAscending ?
                            filteredItemsQuery.OrderBy(item => item.Price) :
                            filteredItemsQuery.OrderByDescending(item => item.Price);
                        break;
                    case "likeCount":
                        filteredItemsQuery = filters.SortAscending ?
                             filteredItemsQuery.OrderBy(item => item.LikeCount) :
                             filteredItemsQuery.OrderByDescending(item => item.LikeCount);
                        break;
                    case "dislikeCount":
                        filteredItemsQuery = filters.SortAscending ?
                             filteredItemsQuery.OrderBy(item => item.DislikeCount) :
                             filteredItemsQuery.OrderByDescending(item => item.DislikeCount);
                        break;
                    case "commentCount":
                        filteredItemsQuery = filters.SortAscending ?
                             filteredItemsQuery.OrderBy(item => item.CommentCount) :
                             filteredItemsQuery.OrderByDescending(item => item.CommentCount);
                        break;
                    case "createdDate":
                        filteredItemsQuery = filters.SortAscending ?
                            filteredItemsQuery.OrderBy(item => item.CreatedDate) :
                            filteredItemsQuery.OrderByDescending(item => item.CreatedDate);
                        break;
                    default:
                        // Handle other sorting filter using Utils.GetPropertyValue
                        filteredItemsQuery = filters.SortAscending ?
                            filteredItemsQuery.OrderBy(item => Utils.GetPropertyValue(item, filters.SortBy)) :
                            filteredItemsQuery.OrderByDescending(item => Utils.GetPropertyValue(item, filters.SortBy));
                        break;
                }
            }

            // Apply paging
            var pagedItems = filteredItemsQuery
                .Skip((filters.PageNumber - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToList(); // Materialize the query

            return new PagedResult<GetArtworkDTO>
            {
                Items = pagedItems,
                PageNumber = filters.PageNumber,
                PageSize = filters.PageSize,
                TotalItems = pagedItems.Count()
            };
        }
    }
}