# OculusEntrainment
[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com) <p align="left"> <a href="https://developer.android.com" target="_blank" rel="noreferrer"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/android/android-original-wordmark.svg" alt="android" width="40" height="40"/> </a> <a href="https://www.w3schools.com/cs/" target="_blank" rel="noreferrer"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/> </a> <a href="https://unity.com/" target="_blank" rel="noreferrer"> <img src="https://www.vectorlogo.zone/logos/unity3d/unity3d-icon.svg" alt="unity" width="40" height="40"/> </a> <a href="https://www.oculus.com/experiences/quest/" target="_blank" rel="noreferrer"> <img src="https://a11ybadges.com/badge?logo=oculus" alt="oculus" width="150" height="40"/> </a></p>

Entrainment Unity VR game that can be used to interact with Audio and Visual entrainment as well as Neurofeedback.

## VR Game word:
- Large ball: Visual entrainment element
- Smaller ball: Neurofeedback element

![](Images/WorldExample.jpg?raw=true)

## System rundown
- Game defaults to Alpha wave entrainment through audio and visual entrainment.
- Game can dynamically update the entrainment stimulus using an AWS API or a similar REST api. More details to follow.

## API integration
- Add a JSON file, `aws_resources.json` under the Resources directory that will assist the Game with pointing to the API, the json has been added to the gitignore file. JSON file should have the following fields:

```json
{
  "authentication_api_key": {},
  "authentication_lambda_url": {},
  "experimentTableArn": {},
  "get_data_api_key": {},
  "get_data_url": {},
  "lambda_bucket_name": {},
  "participant_data_bucket": {},
  "set_data_sqs_url": {}
}
```

- Each JSON field should, futher, consist of the following values with types listed:

```json
{
    "sensitive": bool,
    "type": string,
    "value": string
}
```

- The NetworkController will poll the API to retreive data from the API specified using the `get_data_url` provided in the JSON. It is expected that the `get_data_url` will return a JSON with the following structure and types:

```json
{
    "visual": {
        "colour": string,
        "frequency": string,
    },
    "audio": {
        "baseFrequency": string,
        "entrainmentFrequency": string
    },
    "neurofeedback": {
        "redChannel": string,
        "greenChannel": string
    }
}
```
- A `baseFrequency` audio entrainment setting of -`1` will result in a pink noise audio stimulus. This can be used as a good control stimulus.
- A working API system has been developed and open sourced in the [entrainmentInfrastructure](https://github.com/RC-7/entrainmentInfrastructure) repository. This uses Terraform and, therefore, can be pulled up with minimal effort or technical knowledge.
