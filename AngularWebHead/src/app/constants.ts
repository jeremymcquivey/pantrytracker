export class Constants {
  //public static recipeApi = 'http://localhost:64326/api/';
  //public static stsAuthority = 'https://localhost:5001';

  public static recipeApi = 'https://pantrytrackers-recipeapi-dev.azurewebsites.net/api/';
  public static stsAuthority = 'https://pantrytrackers-identity-dev.azurewebsites.net/';
  
  //public static recipeApi = 'https://pantrytrackers-recipeapi-stage.azurewebsites.net/api/';
  //public static stsAuthority = 'https://pantrytrackers-identity-stage.azurewebsites.net/';

  public static clientId = 'pantrytrackers-ui';
  public static clientScope = 'openid pantrytrackers-api profile';
  public static clientRoot = window.location.hostname === 'localhost' ? 
                                'http://' + window.location.hostname + ':' + window.location.port + '/' :
                                'https://' + window.location.hostname + '/';
}