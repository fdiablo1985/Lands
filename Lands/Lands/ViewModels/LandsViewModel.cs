﻿
namespace Lands.ViewModels
{
    using Lands.Models;
    using Lands.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class LandsViewModel : BaseViewModel
    {

        #region Services
        private ApiService apiservice;
        #endregion


        #region Attributes
        private ObservableCollection<Land> lands;
        #endregion

        #region Properties
        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { SetValue(ref this.lands, value); }
        }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            this.apiservice = new ApiService();
            this.LoadLands();
        }

        #endregion


        #region Methods
        private async void LoadLands()
        {

            var connection = await this.apiservice.CheckConnection();

            if(!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Accept");
                //Navegar para atrás
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            
            var response = await this.apiservice.GetList<Land>(
                "http://restcountries.eu",
                "/rest",
                "/v2/all");

            if(!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error",response.Message,"Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var list = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(list);
        } 
        #endregion


    }
}