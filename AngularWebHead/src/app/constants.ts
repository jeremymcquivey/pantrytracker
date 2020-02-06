export class Constants {
  public static apiRoot = 'http://localhost:2112/api/';
  //public static recipeApi = 'http://localhost:64326/api/';
  public static recipeApi = 'https://pantrytrackers-recipeapi-dev.azurewebsites.net/api/';
  public static stsAuthority = 'https://pantrytrackers-identity-dev.azurewebsites.net/';

  public static clientId = 'pantrytrackers-ui';
  public static clientRoot = window.location.hostname === 'localhost' ? 
                                'http://' + window.location.hostname + ':' + window.location.port + '/' :
                                'https://' + window.location.hostname + '/';
}