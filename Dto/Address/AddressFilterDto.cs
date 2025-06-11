using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class AddressFilterDto : GenericFilterDto
{
    public string? UserNameFilterText { get; set; }
    public string? AddressNameFilterText { get; set; }
    public string? Line1FilterText { get; set; }
    public string? Line2FilterText { get; set; }
    public string? LandmarkFilterText { get; set; }
    public string? VillageFilterText { get; set; }
    public string? CityFilterText { get; set; }
    public string? DistrictFilterText { get; set; }
    public string? StateFilterText { get; set; }
    public string? CountryFilterText { get; set; }
    public string? PincodeFilterText { get; set; }
}
