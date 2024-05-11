interface Object {
  [key: string]: any;
}

export const mapObjectToParams = (obj: Object): string => {
  return Object.keys(obj)
      .filter(key => obj[key] !== undefined)
    .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(obj[key])}`)
    .join('&');
};