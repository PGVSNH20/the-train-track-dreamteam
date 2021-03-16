# Documentation
## TrainEngine API
### Main terms and concept
![Concept ](_assets/concept.png)
* API can handle track map thats origins from one station and "grows" to east
* Tracks can be branches of other tracks and tracks kan merge
* API can handle "unlimited" amount of stations, switches, crossings and tracks.
* Tracks contains at least one link. Links starts att station or switch and ends att station or switch. 
### Track ORM
Handls data related to Tracks:
* Takes input data as file or string
```C#
// reads default file
var trackORM = new TrackORMAdv();

// reads specific file
var trackORM = new TrackORMAdv(@"Data\traintrack3.txt");

// reads string as data input
var trackORM = new TrackORMAdv(""*[1]---[2]"", false);
```
* Renders track map 
```C#
trackORM.PrintTrackMap();
```
![Track Map ](_assets/track_map_render.png)
* Contains methods related to track information:
```C#
// gets the minimal travel time between two station for specific train
trackORM.GetMinTravelTime(trainId, beginStationId, finishStationId)

// gets the minimal travel time between two station based on specific speed
trackORM.GetTravelTime(speed, beginStationId, finishStationId)

// gets lenght between two station
trackORM.GetTrackLength(beginStationId, finishStationId)

// gets trip direction
trackORM.GetTripDirection(beginStationId, finishStationId)
```
### Train ORM
Handls data related to Trains:
* Reads from file
* Writes to file
* Contains methods related to train information
### Station ORM
Handls data related to Trains:
* Reads from file
* Writes to file
### Travel Plan
Handls data related to Travel plan:
* Reads from file
* Writes to file
* Contains methods related to travel plan:
