using CnC.Application.Features.Product.Commands.Create;
using CnC.Application.Validations.ProductValidations;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Moq;

namespace _3D_CnC.UnitTest.ProductValidationTest;

public class CreateProductCommandRequestValidatorTests
{
    private readonly CreateProductCommandRequestValidator _validator;

    public CreateProductCommandRequestValidatorTests()
    {
        _validator = new CreateProductCommandRequestValidator();
    }

    [Fact]
    public void CreateProductCommandRequest_AllFieldsValidation()
    {
        var invalidRequest = new CreateProductCommandRequest
        {
            Name = "",  
            PriceAzn = 0, 
            DiscountedPercent = 150, 
            CategoryId = Guid.Empty, 
            PreviewImageUrl = null! 
        };

        _validator.TestValidate(invalidRequest)
            .ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Product name cannot be null");

        _validator.TestValidate(invalidRequest)
            .ShouldHaveValidationErrorFor(x => x.DiscountedPercent)
            .WithErrorMessage("DiscountedPercent must be between 0 and 100");

        _validator.TestValidate(invalidRequest)
            .ShouldHaveValidationErrorFor(x => x.PriceAzn)
            .WithErrorMessage("PriceAzn must be greater than 0");

        _validator.TestValidate(invalidRequest)
            .ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage("CategoryId cannot be null");
        
        _validator.TestValidate(invalidRequest)
            .ShouldHaveValidationErrorFor(x => x.PreviewImageUrl)
            .WithErrorMessage("Preview image is required");

        var validRequest = new CreateProductCommandRequest
        {
            Name = "Valid Product",
            PriceAzn = 100,
            DiscountedPercent = 20,
            CategoryId = Guid.NewGuid(),
            PreviewImageUrl = Mock.Of<IFormFile>()
        };

        var result = _validator.TestValidate(validRequest);
        result.IsValid.Should().BeTrue();
    }
}

