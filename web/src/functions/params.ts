interface Object {
  [key: string]: any;
}

export const mapObjectToParams = (obj: Object): string => {
  return Object.keys(obj)
    .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(obj[key])}`)
    .join('&');
};