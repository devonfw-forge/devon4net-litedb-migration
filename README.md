[![.NET](https://github.com/BenjaminE98/DevonFwMTS/actions/workflows/dotnet.yml/badge.svg)](https://github.com/BenjaminE98/DevonFwMTS/actions/workflows/dotnet.yml)

Playground for C# development!

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#running-the-project">Running the project</a></li>
    <li><a href="#testing-using-docker">Testing using docker</a></li>
  </ol>
</details>

## Running the project
If you want to run the frontend with the backend and database run `docker-compose up -d` from the root directory.

## Testing using docker:
Change into the corresponding directory using "cd Templates/WebAPI"

Build the dockerfile using:

`docker build -f .\Devon4Net.Application.WebAPI\Dockerfile -t dotnet-backend --target test .`

Execute the unit tests (in docker) using:

`docker run -it --rm --name dotnet-test dotnet-backend`
