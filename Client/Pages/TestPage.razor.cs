using Obra.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using SharedLibrary.Models;

namespace Obra.Client.Pages
{
    public partial class TestPage
    {
        private readonly IBuildingsService _buildingsService;
        private readonly IAreasService _areasService;
        private readonly IActivitiesService _activitiesService;

        public TestPage(IBuildingsService buildingsService, IAreasService areasService, IActivitiesService activitiesService)
        {
            _buildingsService = buildingsService;
            _areasService = areasService;
            _activitiesService = activitiesService;
        }

        private List<Building> buildings { get; set; }
        private Building building { get; set; }
        private string nameBuilding { get; set; } = "building";

        private List<Area> areas { get; set; }
        private Area area { get; set; }
        private string nameArea { get; set; } = "area";

        private List<Activity> activities { get; set; }
        private Activity activity { get; set; }
        private string nameActivity { get; set; } = "activity";

        protected override async Task OnInitializedAsync()
        {
            buildings = await _buildingsService.GetBuildingsAsync();
            areas = await _areasService.GetAreasAsync();
            activities = await _activitiesService.GetActivitiesAsync();
        }

        public async Task GetsId(ChangeEventArgs e, int id, string nameS)
        {
            switch (nameS)
            {
                case "building":
                    building = await _buildingsService.GetBuildingAsync(id);
                    break;
                case "area":
                    area = await _areasService.GetAreaAsync(id);
                    break;
                case "activity":
                    activity = await _activitiesService.GetActivityAsync(id);
                    break;
            }
        }
    }
}
