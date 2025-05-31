using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto;

public class ProductFilterDto : GenericFilterDto
{
    public string? NameFilterText { get; set; }
    public string? DescriptionFilterText { get; set; }
    public string? OriginalPriceFilterText { get; set; }
    public string? DiscountedPriceFilterText { get; set; }
    public string? SubTitleFilterText { get; set; }
    public string? StrachFilterText { get; set; }
    public string? SoftnessFilterText { get; set; }
    public string? TransparencyFilterText { get; set; }
    public string? FabricFilterText { get; set; }
    public string? ColorFilterText { get; set; }
    public string? PrintFilterText { get; set; }
    public string? SizeFilterText { get; set; }
    public string? FeaturesFilterText { get; set; }
    public string? LengthFilterText { get; set; }
    public string? DeliveryPriceFilterText { get; set; }
    public string? WashingInstructionFilterText { get; set; }
    public string? IroningInstructionFilterText { get; set; }
    public string? BleachingInstructionFilterText { get; set; }
    public string? DryingInstructionFilterText { get; set; }
    public string? ProductGroupNameFilterText { get; set; }
}
