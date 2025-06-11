using Data.Models;
using Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo;

public interface IImageRepo : IBaseRepo<Image>
{
    Task InsertImageAsync(Image image, int productID);
}
