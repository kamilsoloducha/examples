import http from 'k6/http';

export let options = {
  stages: [
      // Ramp-up from 1 to 5 virtual users (VUs) in 5s
      { duration: "5s", target: 5 },

      // Stay at rest on 5 VUs for 10s
      { duration: "10s", target: 5 },

      // Ramp-down from 5 to 0 VUs for 5s
      { duration: "5s", target: 0 }
  ]
};

export default function () {
  http.get("http://localhost:5000/WeatherForecast/GetWeatherForecast", {headers: {Accepts: "application/json"}});
};
