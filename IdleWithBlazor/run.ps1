$imageweb = "web:0.1"
$imageserver = "server:0.1"
Write-Output "Cleaning Docker system"
docker system prune --force
Write-Output "Running docker build"

 docker build `
  -t $imageweb `
  --file ./Dockerfile.web `
  --rm `
  .
 docker build `
  -t $imageserver `
  --file ./Dockerfile.server `
  --rm `
  .

Write-Output "Running docker compose up"
docker-compose -f docker-compose.yml up