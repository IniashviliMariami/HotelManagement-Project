using AutoMapper;
using Hotels.Models.Dto;
using Hotels.Models.Dto.Guest;
using Hotels.Models.Dto.GuestReservation;
using Hotels.Models.Dto.Hotel;
using Hotels.Models.Dto.Identity;
using Hotels.Models.Dto.Manager;
using Hotels.Models.Dto.Reservation;
using Hotels.Models.Dto.Room;
using Hotels.Models.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            CreateMap<GuestForCreateDto, Guest>();
            CreateMap<GuestForUpdateDto, Guest>();
            CreateMap<Guest, GuestForGettingDto>();


            //  CreateMap<HotelForCreateDto, Hotel>();
            CreateMap<HotelForCreateDto, Hotel>()
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
              .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
              .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
              .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
              .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId));
            CreateMap<HotelForUpdateingDto, Hotel>();
            CreateMap<Hotel, HotelForGettingDto>();
            CreateMap<Hotel, HotelForGettingDto>();


            CreateMap<Room, RoomForGettingDto>();
            CreateMap<RoomForCreateDto, Room>();
            CreateMap<RoomForUpdateDto, Room>();

            CreateMap<ManagerForCreateDto, Manager>();
            CreateMap<Manager, ManagerForGettingDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<ManagerForUpdateDto, Manager>();

            

            CreateMap<ReservationForGettingDto, Reservation>();
            CreateMap<ReservationFilterDto, Reservation>();
            CreateMap<Reservation, ReservationForGettingDto>()
            .ForMember(dest => dest.Guests, opt => opt.MapFrom(src =>
                src.GuestReservations.Where(gr => gr.Guest != null)
                    .Select(gr => new GuestForGettingDto { Id = gr.Guest.Id })
                    .ToList()));


            CreateMap<Reservation, ReservationForGettingDto>()
            .ForMember(dest => dest.Guests, opt => opt.MapFrom(src =>
                    src.GuestReservations.Select(gr => new GuestReservationForGettingDto
                    {
                        GuestId = gr.GuestId,
                        ReservationId = gr.ReservationId
                    }).ToList()));







            CreateMap<Reservation, ReservationForGettingDto>();
            CreateMap<GuestReservation, GuestReservationForGettingDto>();

            CreateMap<UserDto, ApplicationUser>().ReverseMap();
            CreateMap<RegistrationRequestDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName, options => options.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.Email, options => options.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail, options => options.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FullName));

            CreateMap<ReservationForCreateDto, Reservation>();
            CreateMap<ReservationForCreateDto, Reservation>()
               .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.CheckIn, opt => opt.MapFrom(src => src.CheckIn))
               .ForMember(dest => dest.CheckOut, opt => opt.MapFrom(src => src.CheckOut))
               .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId));

          

            CreateMap<Reservation, ReservationForGettingDto>()
                .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.GuestReservations.Select(gr => gr.Guest)))
                .ForMember(dest => dest.HotelId, opt => opt.MapFrom(src => src.Room.HotelId))
                .ForMember(dest => dest.GuestId, opt => opt.MapFrom(src => src.GuestReservations.Select(gr => gr.Guest.Id).FirstOrDefault())); 

            CreateMap<GuestReservationForGettingDto, GuestReservation>()
            .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.ReservationId))
            .ForMember(dest => dest.GuestId, opt => opt.MapFrom(src => src.GuestId));

            CreateMap<GuestReservation, GuestReservationForGettingDto>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.ReservationId))
                .ForMember(dest => dest.GuestId, opt => opt.MapFrom(src => src.GuestId));

            CreateMap<GuestReservation, GuestReservationForGettingDto>();
            //.ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Reservation.Id));



        }
    }
}
