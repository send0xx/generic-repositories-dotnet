// ReSharper disable ClassNeverInstantiated.Global
using System.ComponentModel.DataAnnotations;

namespace GenericRepositories.SampleApi.Models;

internal sealed record CreateProductRequest(
    [property: Required, MinLength(2), MaxLength(100)] string Name,
    [property: Range(0.01, 1_000_000)] decimal Price);

internal sealed record UpdateProductRequest(
    [property: Required, MinLength(2), MaxLength(100)] string Name,
    [property: Range(0.01, 1_000_000)] decimal Price);
