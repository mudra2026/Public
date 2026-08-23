# Render deployment

1. Create a GitHub repository and push this project.
2. In Render, choose **New -> Blueprint** and select the repository.
3. Render will use `render.yaml` and build the included `Dockerfile`.
4. Add SMTP values in Render Environment. Never commit the SMTP password.
5. Deploy and open the generated `https://...onrender.com` URL.

The free plan can sleep after inactivity. `App_Data/enquiries.json` is local container storage and is not permanent on free hosting. Use a hosted database or email notifications for production enquiries.
