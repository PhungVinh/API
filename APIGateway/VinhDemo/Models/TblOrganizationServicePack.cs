using System;
using System.Collections.Generic;

namespace OrganizationManagement.Models
{
    public partial class TblOrganizationServicePack
    {
        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public int? ServicePackId { get; set; }
    }
}
