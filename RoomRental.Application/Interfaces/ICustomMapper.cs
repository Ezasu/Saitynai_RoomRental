using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomRental.Application.Interfaces
{
    interface ICustomMapper
    {
        void CreateMappings(Profile configuration);
    }
}
