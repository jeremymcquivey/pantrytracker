using AutoMapper;
using PantryTracker.Model.Grocery;
using PantryTracker.Model.Extensions;

namespace RecipeAPI
{
    /// <summary>
    /// This is a temporary class meant to leverage AutoMapper to bridge the gap between string and numeric quantity conversions
    /// until I get around to updating the UI to do it the right way.
    /// </summary>
    public class ShimmingProfile : Profile
    {
        /// <summary>
        /// </summary>
        public ShimmingProfile()
        {
            CreateMap<ListItemViewModel, ListItem>()
                .ForMember(p => p.Quantity, opt => opt.MapFrom(src => src.Quantity.ToNumber(1)));
        }
    }
}
