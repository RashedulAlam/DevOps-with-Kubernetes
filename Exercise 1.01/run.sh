#!/usr/bin/env bash

cd RandomStringGenerator
docker build -t rsg .

k3d cluster delete ex-101 && k3d cluster create ex-101 --api-port 127.0.0.1:6443
k3d image import rsg --cluster ex-101
kubectl create deployment random-string-generator --image=rsg
