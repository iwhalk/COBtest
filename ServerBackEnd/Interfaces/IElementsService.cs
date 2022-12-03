﻿using SharedLibrary.Models;
using SharedLibrary;

namespace ApiGateway.Interfaces
{
    public interface IElementsService
    {
        Task<ApiResponse<Element>> GetElementAsync(int id);
        Task<ApiResponse<List<Element>>> GetElementsAsync();
        Task<ApiResponse<Element>> PostElementAsync(Element element);
    }
}