import requests

host = "https://api.srgssr.ch"
headers = {
    'Authorization': 'Bearer k2GgpR2YXaM86Ao6UibBoYpTQzVI',
}

def get_geolocation(latitude=46.925727, longitude=7.467183, printout=True):
    """Returns Geolocation data from SRF API

    Note:
        You will need the "id" (forst one in json) for further usage.
    Returns:
        str: json.loads() object
    """
    params = {
        'latitude': latitude,
        'longitude': longitude,
    }
    uri = "srf-meteo/geolocations"
    response = requests.get(f'{host}/{uri}', params=params, headers=headers)
    
    if printout:
        print("-----Return Code-----")
        print(response)
        print("-----return as json-----")
        print(response.json())
        
    return response.json()

def get_forecast(geolocationid, printout=True):
    """Returns Weather forecast data from SRF API

    Note:
        You will need the "id" from get_geolocation() for further usage.
        
    Returns:
        str: Forecast in prosa
    """

    uri = f"srf-meteo/forecast/{geolocationid}"
    response = requests.get(f'{host}/{uri}', headers=headers)
    
    if printout:
        print("-----Return Code-----")
        print(response)
        print("-----return as json-----")
        print(response.json())
        
    return response.json()

get_forecast(geolocationid="46.9246,7.4741")