# Listen to htmx:configRequest event to send parameters 

This example shows how to listen to `htmx:configRequest` to pass parameters to all supported HTTP verbs ([doc](https://htmx.org/events/#htmx:configRequest))

> This event is triggered after htmx has collected parameters for inclusion in the request. It can be used to include or update the parameters that htmx will send

```js
    document.addEventListener("htmx:configRequest", (evt) => {
        evt.detail.parameters["Name"] = "John Doe";
    });
```

On `GET` and `DELETE`, the parameters are accessible via `Request.Query`. For the rest, you can access the parameters via `Request.Form`.