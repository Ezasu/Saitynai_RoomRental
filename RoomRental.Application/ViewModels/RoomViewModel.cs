using AutoMapper;
using RoomRental.Application.Interfaces;
using RoomRental.Domain.Entities.RoomRental;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomRental.Application.ViewModels
{
    public class RoomViewModel : ICustomMapper
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Room, RoomViewModel>();
        }
    }
}
