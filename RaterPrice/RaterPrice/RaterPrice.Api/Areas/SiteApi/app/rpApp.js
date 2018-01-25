
angular.module('rpApp', [
  'ngRoute',
  'ui.router'
]).config(function ($stateProvider, $locationProvider) {
 
  
    $stateProvider
      .state('main',
      {
          url: '/home/index',
        templateUrl: 'Areas/SiteApi/app/views/Home.html',
        controller: 'goodsController'
      })
      .state('catalog',
      {
          url: '/catalog',
          templateUrl: 'Areas/SiteApi/app/views/Catalog.html',
        
      })
      .state('contacts',
      {
          url: '/contacts',
          templateUrl: 'Areas/SiteApi/app/views/Contacts.html'    
      })
        .state('login',
      {
          url: '/login',
          templateUrl: 'Areas/SiteApi/app/views/Login.html',
          controller: 'loginController'
      }).state('register',
      {
          url: '/register',
          templateUrl: 'Areas/SiteApi/app/views/register.html',
          controller: 'registerController'
      });
  });

