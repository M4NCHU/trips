using AutoMapper;
using backend.Application.Services.Interfaces;
using backend.Domain.DTOs;
using backend.Domain.Filters;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IBaseUrlService _baseUrlService;
        private readonly string _baseUrl;
        private readonly ILogger<GeoLocationService> _logger;
        private readonly IMapper _mapper;

        public GeoLocationService(
            IUnitOfWork unitOfWork,
            IImageService imageService,
            IWebHostEnvironment hostEnvironment,
            IBaseUrlService baseUrlService,
            ILogger<GeoLocationService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
            _logger = logger;
            _mapper = mapper;
        }

        

    }
}
