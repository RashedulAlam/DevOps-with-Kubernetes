#!/usr/bin/env bash

docker run --name my-postgres -p 5432:5432 -d -p 5433:5432 postgres
