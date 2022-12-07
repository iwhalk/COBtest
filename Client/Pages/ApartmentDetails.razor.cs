using Microsoft.AspNetCore.Components;
using Obra.Client.Interfaces;
using Obra.Client.Stores;
using SharedLibrary.Models;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;

namespace Obra.Client.Pages
{
    public partial class ApartmentDetails : ComponentBase
    {
        private readonly ApplicationContext _context;
        private readonly IApartmentsService _apartmentsService;
        private readonly IActivitiesService _activitiesService;
        private readonly IElementsService _elementsService;
        private readonly ISubElementsService _subElementsService;

        private List<Apartment> apartments { get; set; }
        private List<Activity> activities { get; set; }
        private List<Element> elements { get; set; }
        private List<SubElement> subElements { get; set; }

        private List<int> _idsAparmentSelect { get; set; } = new();
        private List<int> _idsActivitiesSelect { get; set; } = new();
        private List<int> _idsElementsSelect { get; set; } = new();
        private List<int> _idsSubElementsSelect { get; set; } = new(); 

        private bool alert = false;
        private int activityIdAux = 0;
        private int elementIdAux = 0;

        public ApartmentDetails(ApplicationContext context, IApartmentsService apartmentsService, IActivitiesService activitiesService, IElementsService elementsService, ISubElementsService subElementsService)
        {
            _context = context;
            _apartmentsService = apartmentsService;
            _activitiesService = activitiesService;
            _elementsService = elementsService;
            _subElementsService = subElementsService;
        }

        protected async override Task OnInitializedAsync()
        {
            apartments = await _apartmentsService.GetApartmentsAsync();
            activities = await _activitiesService.GetActivitiesAsync();
        }

        public async Task AddIdSelect(int id, int filter)
        {
            if (filter == 1)
            {
                if (!_idsAparmentSelect.Contains(id))
                {
                    _idsAparmentSelect.Add(id);
                }
                else if (_idsAparmentSelect.Count() == 1)
                {
                    _idsAparmentSelect.Remove(id);

                    _idsActivitiesSelect.Clear();
                
                    if (elements != null)
                    {
                        elements.Clear();
                        _idsElementsSelect.Clear();

                        if (subElements != null)
                        {
                            subElements.Clear();
                            _idsSubElementsSelect.Clear();
                        }
                    }
                }
                else
                {
                    _idsAparmentSelect.Remove(id);
                }
            }
            else if (filter == 2)
            {
                if (_idsAparmentSelect != null && _idsAparmentSelect.Count() > 0)
                {
                    if (_idsActivitiesSelect.Count() < 1)
                    {
                        if (!_idsActivitiesSelect.Contains(id))
                        {
                            _idsActivitiesSelect.Add(id);
                            activityIdAux = id;
                            elements = await _elementsService.GetElementsAsync(id);
                        }
                        else
                        {
                            _idsActivitiesSelect.Remove(id);

                            if (elements != null)
                            {
                                elements.Clear();
                                _idsElementsSelect.Clear();

                                if (subElements != null)
                                {
                                    subElements.Clear();
                                    _idsSubElementsSelect.Clear();
                                }
                            }
                        }
                    }
                    else if (activityIdAux == id)
                    {
                        _idsActivitiesSelect.Remove(id);

                        if (elements != null)
                        {
                            elements.Clear();
                            _idsElementsSelect.Clear();

                            if (subElements != null)
                            {
                                subElements.Clear();
                                _idsSubElementsSelect.Clear();
                            }
                        }
                    }
                }
                else
                {
                    alert = true;
                }
            }
            else if (filter == 3)
            {
                if (_idsElementsSelect.Count() < 1)
                {
                    if (!_idsElementsSelect.Contains(id))
                    {
                        _idsElementsSelect.Add(id);
                        elementIdAux = id;
                        subElements = await _subElementsService.GetSubElementsAsync(id);
                    }
                    else
                    {
                        _idsElementsSelect.Remove(id);

                        if (subElements != null)
                        {
                            subElements.Clear();
                            _idsSubElementsSelect.Clear();
                        }
                    }
                }
                else if (elementIdAux == id)
                {
                    _idsElementsSelect.Remove(id);

                    if (subElements != null)
                    {
                        subElements.Clear();
                        _idsSubElementsSelect.Clear();
                    }
                }
            }
            else if (filter == 4)
            {
                if (!_idsSubElementsSelect.Contains(id))
                {
                    _idsSubElementsSelect.Add(id);
                }
                else
                    _idsSubElementsSelect.Remove(id);
            }
        }

        public async Task ShowMenssage() => alert = false;
    }
}
