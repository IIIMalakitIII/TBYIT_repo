using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ave.DAL.Entities;

namespace Ave.BLL.Interface
{
    public interface IArtificialOrganService
    {
        Task<string> DiagnosticArtificialOrgan(int id, DateTime from, DateTime to);

        Task<ArtificialOrgan> CreateArtificialOrgan(ArtificialOrgan model);

        Task AddOrganData(OrganData model, Guid key);

        Task<List<ConnectHistory>> GetConnectHistory(int organId, string userId);

        Task<ArtificialOrgan> GetOrganById(int id, string userId, string role);

        Task<List<ArtificialOrgan>> GetPatientOrgansById(int patientId, string id, string role, string userId);

        Task<List<OrganData>> GetOrganData(int id, string userId, string role);

        Task UpdateOrgan(ArtificialOrgan model);

        Task GenerateOrganKey(int id, string userId, string role);

        Task<List<ArtificialOrganKey>> GetOrganKeys(int id);

        Task CreateConfidant(string userId, int doctorId);

        Task<List<Confidant>> GetConfidants(string userId);

        Task DeleteConfidant(string userId, int confidantId);
    }
}
