using Domain.Enterprise.models;
using Domain.StoreSystem.models;

namespace Domain.Enterprise.Models
{
    public class Enterprise
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public List<Store> Stores { get; set; }
        public List<EnterpriseSupplier> EnterpriseSupplier { get; set; }
    }
}
