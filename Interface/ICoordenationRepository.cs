using acadgest.Dto.Coordenation;
using acadgest.Models.Coordenations;
using acadgest.Models.Results;

namespace acadgest.Interface
{
    public interface ICoordenationRepository
    {
        public Task<List<Coordenation>> GetAllAsync();
        public Task<Coordenation?> GetByIdAsync(Guid id);
        public Task<Coordenation?> CreateAsync(Coordenation coordenation);
        public Task<Coordenation?> UpdateAsync(Guid id, UpdateCoordenationDto updateCoordenationDto);
        public Task<SetCoordinatorResult> SetCoordinator(Guid coordenationId, Guid coordinatorId);
        public Task<bool> ExistsAsync(Guid id);
        public Task<DeleteResults> DeleteAsync(Guid id);
    }
}