#! /bin/bash

kubectl apply -f file-service/file-service-deployment.yaml
kubectl apply -f auth-service/auth-service-deployment.yaml
kubectl apply -f api-gateway/api-gateway-ingress.yaml