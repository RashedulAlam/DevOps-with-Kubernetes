#!/usr/bin/env bash

docker run --name my-postgres -e POSTGRES_PASSWORD="ralam@@@123" -d postgres
