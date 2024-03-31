export const getToken = () => {
  const token = localStorage.getItem('token');

  if (token) {
    return JSON.parse(token);
  }
  else {
    return undefined;
  }
}