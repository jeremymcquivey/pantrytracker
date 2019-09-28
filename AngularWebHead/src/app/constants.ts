export class Constants {
  public static apiRoot = 'http://localhost:2112/api/';
  //public static recipeApi = 'http://localhost:64325/api/';
  public static recipeApi = 'https://pantrytrackers-recipeapi-dev.azurewebsites.net/api/';
  public static stsAuthority = 'https://pantrytracker-identity-dev.azurewebsites.net/';

  public static clientId = 'pantrytrackers-ui';
  public static clientRoot = window.location.hostname === 'localhost' ? 
                                'http://' + window.location.hostname + ':' + window.location.port + '/' :
                                'https://' + window.location.hostname + '/';
}