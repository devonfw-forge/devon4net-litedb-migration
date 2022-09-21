using System;
using System.Collections.Generic;
using Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Dto;
using Devon4Net.Application.WebAPI.Implementation.Domain.Entities;

namespace Devon4Net.Application.WebAPI.Implementation.Business.DishManagement.Converters
{
    public class DishConverter
    {
        /// <summary>
        /// Transforms entity object to Dto object
        /// </summary>
        /// <param name="item">Entity item to be transformed to api Dto</param>
        /// <returns>API Dto</returns>
        public static DishDtoResult EntityToApi(Dish item)
        {
            if (item == null) return null;

            return new DishDtoResult
            {
                dish = DishToApi(item),
                categories = GetDishCategories(item.Categories),
                image = GetImageDtoFromImage(item.Image),
                extras = GetDishExtras(item.Ingredients),
            };
        }

        private static List<ExtraDto> GetDishExtras(ICollection<Ingredient> itemDishIngredient)
        {
            var result = new List<ExtraDto>();

            if (itemDishIngredient == null) return result;

            try
            {
                foreach (var item in itemDishIngredient)
                {
                    result.Add(new ExtraDto
                    {
                        Description = item.Description,
                        Price = item.Price,
                        Name = item.Name
                    });
                }
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
            }

            return result;
        }

        private static List<CategoryDto> GetDishCategories(ICollection<Category> itemDishCategory)
        {
            var result = new List<CategoryDto>();

            if (itemDishCategory == null) return result;

            try
            {
                foreach (var item in itemDishCategory)
                {
                    result.Add(new CategoryDto
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Name = item.Name,
                    });

                }
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
            }

            return result;
        }

        private static DishDto DishToApi(Dish item)
        {
            return new DishDto
            {
                Id = item.Id,
                Description = item.Description,
                Name = item.Name,
                Price = item.Price,
                //ImageId = item.IdImage,
                ModificationCounter = 0
            };
        }

        private static ImageDto GetImageDtoFromImage(Image image)
        {
            if (image == null) return new ImageDto();
            var result = new ImageDto();

            try
            {
                result = new ImageDto
                {
                    Content = image.Content,
                    //ModificationCounter = image.ModificationCounter,
                    MimeType = image.MimeType,
                    Name = image.Name,
                    //Id = image.Id,
                    ContentType = image.ContentType == 0 ? "Binary" : "Url",
                    Revision = 1
                };
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
            }


            return result;
        }
    }
}
