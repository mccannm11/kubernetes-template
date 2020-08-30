## Kubernetes template

### Ingress
Currently using the all-popular nginx Ingress conroller for kubernetes https://kubernetes.github.io/ingress-nginx/

#####
* When is perimeter authentication enough?

###### Others to consider: 
* https://www.envoyproxy.io/
* https://haproxy-ingress.github.io/
* https://konghq.com/solutions/kubernetes-ingress/

### auth-service
A .net core based authentication service that uses cookie authentication to generate JWT's for authentication in the other services.

##### Thoughts
* Consider trade off of perimeter vs distributed auth
* Currently this is a single point of failure
* Explore side care authentication https://www.thoughtworks.com/radar/techniques/sidecars-for-endpoint-security
* https://medium.com/@marcus.cavalcanti/stateless-authentication-for-microservices-9914c3529663

### file-service
This is a node app that manages file uploads and downloads

###### Goals
* Handle large (500MB+) files
* Handling of client disconnect
* Graceful handling of inprogress uploads during deploy/appfailure 
* Keeping a log of file access
* Cron job to move less accessed files to cheaper S3 tiers
* What file types can you support diffing in?

### Visibility
* Logging
* Service health metrics
* Exception alerting


### Deployment
* Currently only running this locally with minikube
* Apply all the manifests with `local-deploy-sh` 