import requests

example_forecast="""{
    "geolocation": {
        "id": "46.9246,7.4741",
        "lat": 46.9246,
        "lon": 7.4741,
        "station_id": "S19038",
        "timezone": "Europe/Zurich",
        "default_name": "Bodenacher",
        "alarm_region_id": "1",
        "alarm_region_name": "Aare-/Gürbetal",
        "district": "Muri bei Bern",
        "geolocation_names": [
            {
                "district": "Muri bei Bern",
                "id": "16d8cca5fb283ca17975165baf28e429",
                "location_id": "417258679",
                "type": "city",
                "language": 0,
                "translation_type": "orig",
                "name": "Bodenacher",
                "country": "Schweiz",
                "province": "Bern",
                "inhabitants": 10,
                "height": 508,
                "ch": 1
            }
        ]
    },
    "forecast": {
        "day": [
            {
                "local_date_time": "2022-05-19T00:00:00+02:00",
                "TX_C": 28,
                "TN_C": 12,
                "PROBPCP_PERCENT": 48,
                "RRR_MM": 1,
                "FF_KMH": 7,
                "FX_KMH": 33,
                "DD_DEG": 280,
                "SUNSET": 2103,
                "SUNRISE": 549,
                "SUN_H": 9,
                "SYMBOL_CODE": 12,
                "type": "day",
                "min_color": {
                    "temperature": 12,
                    "background_color": "#a4ca2c",
                    "text_color": "#000000"
                },
                "max_color": {
                    "temperature": 28,
                    "background_color": "#fcac04",
                    "text_color": "#000000"
                }
            },
            {
                "local_date_time": "2022-05-20T00:00:00+02:00",
                "TX_C": 31,
                "TN_C": 13,
                "PROBPCP_PERCENT": 15,
                "RRR_MM": 2,
                "FF_KMH": 9,
                "FX_KMH": 39,
                "DD_DEG": 280,
                "SUNSET": 2104,
                "SUNRISE": 548,
                "SUN_H": 11,
                "SYMBOL_CODE": 10,
                "type": "day",
                "min_color": {
                    "temperature": 13,
                    "background_color": "#b4d21f",
                    "text_color": "#000000"
                },
                "max_color": {
                    "temperature": 31,
                    "background_color": "#fc8c04",
                    "text_color": "#000000"
                }
            }
        ]
    }
}"""


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

def get_forecast(geolocationid, getonlydays=True, printout=True):
    """Returns Weather forecast data from SRF API

    Note:
        You will need the "id" from get_geolocation() for further usage.
        
    Returns:
        str: Forecast in prosa
    """

    uri = f"srf-meteo/forecast/{geolocationid}"
    response = requests.get(f'{host}/{uri}', headers=headers)
    data = response.json()
    #transform data, extract only the days count object.forecast.day[all]
    if getonlydays:
        print("extracting day forecast for db")
        try:
            data['forecast'].pop('60minutes')
            data['forecast'].pop('hour')
            print("truncated data")
        except:
            print("no data to truncate")
            
    if printout:
        print("-----Return Code-----")
        print(response)
        print("-----return as json-----")
        print(data)
        
    return data

#get_forecast(geolocationid="46.9246,7.4741")

def testing():
    import json

    example_file = open(r"C:\Users\TABSCDAQ\Documents\GitHub\Hackathon2022TeamBlue\sourcing\example_jsons\forecast.json")
    example_forecast = json.load(example_file)

    extrated_data = None

    for day in example_forecast['forecast']['day']:
        print(day)
        
    print(example_forecast)
        

# Luzern, Mattenhof = lat=47.025860 long=8.299820 (grob), id = 47.0274,8.3020
# Zürich, Pfingstweidstrasse = lat=47.389810 long=8.511140 (grob), id= 47.3930,8.5047
# Bern, Worblaufen = lat=46.969300 long=7.470520 (grob), id= 46.9679,7.4711
# Genf, Richard Wagner = lat=46.217560 long=6.141990 (grob), id=46.2166,6.1364
# Murtensee = lat=46.931214 long=7.081948 (grob), id= 46.9184,7.0931
#

location_ids=["47.0274,8.3020", "47.3930,8.5047", "46.9679,7.4711", "46.2166,6.1364", "46.9184,7.0931"]
for id in location_ids:
    print("-----")
    get_forecast(id, getonlydays=True, printout=True)
    print("-----")
    