using SoftAir.Data.Interfaces;

namespace SoftAir.Data.Domain.Aircraft
{
    public class Aircraft : Entity, IEntity<int>
    {
        /// <summary>
        /// Name Aircraft
        /// </summary>
        public string Name { get; set; }
    }
}
