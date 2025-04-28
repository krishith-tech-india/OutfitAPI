using Core;
using Core.AppSettingConfigs;
using Data.Models;
using Dto;
using Mapper;
using Microsoft.Extensions.Options;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class AddressService : IAddressService
{
    private readonly IAddressRepo _addressRepo;
    private readonly IAddressMapper _addressMapper;
    private readonly IUserRepo _userRepo;
    public AddressService(
        IAddressRepo addressRepo,
        IAddressMapper addressMapper,
        IUserRepo userRepo
    )
    {
        _addressRepo = addressRepo;
        _addressMapper = addressMapper;
        _userRepo = userRepo;
    }
    public async Task<AddressDto> GetAddressByIdAsync(int id)
    {
        return _addressMapper.GetAddressDto(await _addressRepo.GetAddressByIdAsync(id));
    }

    public async Task<List<AddressDto>> GetAddressByUserIdAsync(int UserId)
    {
        if (!await _userRepo.CheckIsUserIdExistAsync(UserId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"User Id {UserId} is not exist");
        var Address = await _addressRepo.GetAddressByUserIdAsync(UserId);
        return Address.Select(x => _addressMapper.GetAddressDto(x)).ToList();
    }

    public async Task AddAddressAsync(AddressDto addressDto)
    {
        if (!await _userRepo.CheckIsUserIdExistAsync(addressDto.UserId))
            throw new ApiException(System.Net.HttpStatusCode.NotFound, $"User Id {addressDto.UserId} is not exist");
        var addressEntity = _addressMapper.GetEntity(addressDto);
        await _addressRepo.InsertUserAsync(addressEntity);
    }


}
