using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapper;

public class ProductMapper : IProductMapper
{
    public Product GetEntity(ProductDto productDto)
    {
        return new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            OriginalPrice = productDto.OriginalPrice,
            DiscountedPrice = productDto.DiscountedPrice,
            SubTitle = productDto.SubTitle,
            Strach = productDto.Strach,
            Softness = productDto.Softness,
            Transparency = productDto.Transparency,
            Fabric = productDto.Fabric,
            Color = productDto.Color,
            Print = productDto.Print,
            Size = productDto.Size,
            Features = productDto.Features,
            Length = productDto.Length,
            DeliveryPrice = productDto.DeliveryPrice,
            WashingInstruction = productDto.WashingInstruction,
            IroningInstruction = productDto.IroningInstruction,
            BleachingInstruction = productDto.BleachingInstruction,
            DryingInstruction = productDto.DryingInstruction,
            ProductGroupId = productDto.ProductGroupId
        };
    }

    public ProductDto GetProductDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            OriginalPrice = product.OriginalPrice.Value,
            DiscountedPrice = product.DiscountedPrice.Value,
            SubTitle = product.SubTitle,
            Strach = product.Strach,
            Softness = product.Softness,
            Transparency = product.Transparency,
            Fabric = product.Fabric,
            Color = product.Color,
            Print = product.Print,
            Size = product.Size.Value,
            Features = product.Features,
            Length = product.Length,
            DeliveryPrice = product.DeliveryPrice.Value,
            WashingInstruction = product.WashingInstruction,
            IroningInstruction = product.IroningInstruction,
            BleachingInstruction = product.BleachingInstruction,
            DryingInstruction = product.DryingInstruction,
            ProductGroupId = product.ProductGroupId.Value
        };
    }

    public Image GetEntity(ImageDto imageDto)
    {
        return new Image
        {
            Url = imageDto.Url,
            ProductId = imageDto.ProductId,
            ImageTypeId = imageDto.ImageTypeId,
        };
    }

    public ImageDto GetImageDto(Image image)
    {
        return new ImageDto
        {
            Id = image.Id,
            Url = image.Url,
            ProductId = image.ProductId.Value,
            ImageTypeId = image.ImageTypeId.Value,

        };
    }
}
