import { HubConnectionBuilder }  from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
  .withUrl('http://localhost:5050/hubs/social', {
    accessTokenFactory: () => getAuthToken()
  })
  .build();

connection.on("ReceiveMessage", (user, message) => {
  console.log(user);
  console.log(message);
})

document.querySelector<HTMLButtonElement>('#callHubBtn')!.addEventListener('click', async() => {
  try {
    await connection.invoke("SendMessage", 'gonno', 'mocha');
  } catch (err) {
      console.error(err);
  }
});

async function start() {
  try {
      await connection.start();
      console.log("SignalR Connected.");
  } catch (err) {
    console.log(err);
  }
};

async function getAuthToken(): Promise<string> {
  const body = new URLSearchParams();
  body.set('client_id', 'ro.client');
  body.set('client_secret', 'secret');
  body.set('grant_type', 'password');
  body.set('username', 'Supervisor');
  body.set('password', 'Supervisor');
  const authUrl = 'http://localhost:5050/connect/token';
  const response = await fetch(authUrl, {
    method: 'POST',
    headers: {
      'content-Type': 'application/x-www-form-urlencoded',
    },
    body: body,
  });
  const { access_token } = await response.json() as { access_token: string };
  return access_token;
}

start();
