# GmailEmailFilterManager

A self-hosted web application to manage Gmail email filters. You add emails and it will create a string with all emails separated with " OR " to use it in Gmail in a filter/label.

***

## Features

- Manage Gmail filters and labels through a web interface
- Self-hosted – your data stays on your own server
- Docker-based deployment (no .NET installation required)
- Secured with username/password login

***

## Quick Start (Docker)

### 1. Create project folder

```bash
mkdir -p ~/docker/gmailemailfiltermanager/data
cd ~/docker/gmailemailfiltermanager
```

### 2. Create `.env` file

```bash
nano .env
```

```env
ADMIN_USERNAME=admin
ADMIN_PASSWORD=YourSecurePassword!
```

```bash
chmod 600 .env
```

### 3. Create `docker-compose.yml`

```yaml
services:
  emailmanager:
    image: ghcr.io/svenxp/gmailfilter:latest
    container_name: email-manager
    ports:
      - "2180:8080"
    volumes:
      - ./data:/app/App_Data
    env_file:
      - .env
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:8080
      - AdminUser__Username=${ADMIN_USERNAME}
      - AdminUser__Password=${ADMIN_PASSWORD}
    restart: unless-stopped
```

### 4. Start

```bash
docker compose pull
docker compose up -d
```

Open in browser: `http://localhost:2180`

***

## Updating

When a new version is available, pull the latest image and restart:

```bash
cd ~/docker/gmailemailfiltermanager
docker compose pull
docker compose up -d
docker image prune -f
```

***

## Folder Structure

```
~/docker/gmailemailfiltermanager/
├── docker-compose.yml    ← Container configuration
├── .env                  ← Credentials (never commit this!)
└── data/                 ← Persistent app data (filters, lists)
```

***

## License

MIT
